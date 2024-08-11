using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

public class RulesService
{
    private readonly string _filePath = "rules.json";
    private JObject _rules;
    private DateTime _lastReadTime;
    private readonly Dictionary<string, PropertyInfo> _propertyCache;
    private readonly object _lock = new object();

    public RulesService()
    {
        _propertyCache = typeof(Appointment).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                             .Where(p => p.CanWrite)
                                             .ToDictionary(p => p.Name, p => p);
        LoadRules();
    }

    private void LoadRules()
    {
        lock (_lock)
        {
            var fileInfo = new FileInfo(_filePath);
            if (_rules == null || fileInfo.LastWriteTime > _lastReadTime)
            {
                _rules = JObject.Parse(File.ReadAllText(_filePath));
                _lastReadTime = fileInfo.LastWriteTime;
            }
        }
    }

    public void ApplyRules(Appointment appointment)
    {
        LoadRules();

        var calculatedHours = (appointment.ExitTime - appointment.TimeEntry).TotalHours;

        var appointmentData = new Dictionary<string, object>
        {
            { "Id", appointment.Id },
            { "TaskId", appointment.TaskId },
            { "TimeEntry", appointment.TimeEntry },
            { "ExitTime", appointment.ExitTime },
            { "CalculatedHours", calculatedHours },
            { "IsApproved", appointment.IsApproved },
            { "EmployeeType", appointment.EmployeeType },
            { "RejectReason", appointment.RejectReason ?? string.Empty },
            { "ProjectId", appointment.ProjectId }
        };

        foreach (var rule in _rules["Rules"])
        {
            bool ruleMatches = true;

            foreach (var condition in rule["Conditions"])
            {
                string field = (string)condition["Field"];
                string operatorStr = (string)condition["Operator"];
                object conditionValue = ConvertValue(condition["Value"]);

                if (!appointmentData.ContainsKey(field))
                {
                    throw new InvalidOperationException($"Field {field} is missing.");
                }

                var fieldValue = appointmentData[field];

                if (!EvaluateCondition(fieldValue, operatorStr, conditionValue))
                {
                    ruleMatches = false;
                    break;
                }
            }

            if (ruleMatches)
            {
                foreach (var action in rule["Actions"])
                {
                    string field = (string)action["Field"];
                    if (_propertyCache.TryGetValue(field, out var property))
                    {
                        var value = ConvertValue(action["Value"]);
                        property.SetValue(appointment, value);
                    }
                }
            }
        }
    }

    private bool EvaluateCondition(object fieldValue, string op, object value)
    {
        var convertedFieldValue = ConvertValue(fieldValue);
        var convertedValue = ConvertValue(value);

        switch (op)
        {
            case "Equals":
                return Equals(convertedFieldValue, convertedValue);
            case "LessThan":
                return Convert.ToDouble(convertedFieldValue) < Convert.ToDouble(convertedValue);
            case "GreaterThan":
                return Convert.ToDouble(convertedFieldValue) > Convert.ToDouble(convertedValue);
            case "GreaterThanOrEqual":
                return Convert.ToDouble(convertedFieldValue) >= Convert.ToDouble(convertedValue);
            case "LessThanOrEqual":
                return Convert.ToDouble(convertedFieldValue) <= Convert.ToDouble(convertedValue);
            case "In":
                return ((IEnumerable<object>)convertedValue).Contains(convertedFieldValue);
            case "NotIn":
                return !((IEnumerable<object>)convertedValue).Contains(convertedFieldValue);
            case "Between":
                var range = ((IEnumerable<object>)convertedValue).ToArray();
                if (range.Length == 2)
                {
                    var lowerBound = Convert.ToDouble(range[0]);
                    var upperBound = Convert.ToDouble(range[1]);
                    return Convert.ToDouble(convertedFieldValue) >= lowerBound && Convert.ToDouble(convertedFieldValue) <= upperBound;
                }
                throw new Exception("Operador 'Between' deve ter exatamente dois valores.");
            default:
                throw new Exception("Operador não suportado.");
        }
    }

    private object ConvertValue(object value)
    {
        return value is JValue jValue ? jValue.Value : value;
    }
}