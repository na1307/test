using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace EdgeModTool;

internal static class XHelper {
    public static XElement? ElementCaseInsensitive(this XContainer container, XName name) => container.ElementsCaseInsensitive(name).FirstOrDefault();

    public static IEnumerable<XElement> ElementsCaseInsensitive(this XContainer container, XName name)
        => container.Elements().Where(e => name.LocalName.Equals(e.Name.LocalName, StringComparison.InvariantCultureIgnoreCase));

    public static IEnumerable<XElement> ElementsCaseInsensitive(this IEnumerable<XContainer> containers, XName name)
        => containers.Elements().Where(e => name.LocalName.Equals(e.Name.LocalName, StringComparison.InvariantCultureIgnoreCase));

    public static XAttribute? AttributeCaseInsensitive(this XElement element, XName name) => element.AttributesCaseInsensitive(name).FirstOrDefault();

    public static IEnumerable<XAttribute> AttributesCaseInsensitive(this XElement element, XName name)
        => element.Attributes().Where(a => name.LocalName.Equals(a.Name.LocalName, StringComparison.InvariantCultureIgnoreCase));

    public static IEnumerable<XAttribute> AttributesCaseInsensitive(this IEnumerable<XElement> elements, XName name)
        => elements.Attributes().Where(a => name.LocalName.Equals(a.Name.LocalName, StringComparison.InvariantCultureIgnoreCase));

    public static XDocument Load(string path) => XDocument.Parse(File.ReadAllText(path));

    public static void AddIfNotEmpty(this XElement element, XElement child) {
        if (child.HasElements || child.HasAttributes) {
            element.Add(child);
        }
    }

    public static XElement GetElement(this XContainer container, XName name) {
        var r = container.ElementCaseInsensitive(name);

        return r ?? throw new FormatException();
    }

    public static string? GetAttributeValue(this XElement element, XName name) => element.AttributeCaseInsensitive(name)?.Value;

    public static T? GetAttributeValue<T>(this XElement element, XName name) {
        var type = typeof(T);

        if (type.IsSubclassOf(typeof(Enum))) {
            return element.GetAttributeValueEnum<T>(name);
        }

        if (type == typeof(string)) {
            return (T?)(object?)element.GetAttributeValue(name);
        }

        if (IsNullable(type)) {
            return GetAttributeValueWithDefault(element, name, (T?)(object?)null);
        }

        var parse = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, [typeof(string)], null);

        if (parse is null || parse.ReturnType != type) {
            throw new NotSupportedException("You must define a public static T Parse(string) method before using GetAttributeValueWithDefault<T>!");
        }

        return (T?)parse.Invoke(null, [element.GetAttributeValue(name)]);
    }

    public static void GetAttributeValue<T>(this XElement element, out T? result, XName name) {
        if (typeof(T).IsSubclassOf(typeof(Enum))) {
            element.GetAttributeValueEnum(out result, name);
        } else {
            result = element.GetAttributeValue<T>(name);
        }
    }

    public static T? GetAttributeValueWithDefault<T>(this XElement element, XName name, T? defaultValue = default) {
        var type = typeof(T);

        if (type.IsSubclassOf(typeof(Enum))) {
            return element.GetAttributeValueEnumWithDefault(name, defaultValue);
        }

        var str = element.GetAttributeValue(name);

        if (string.IsNullOrWhiteSpace(str)) {
            return defaultValue;
        }

        while (IsNullable(type)) {
            type = type.GetGenericArguments()[0];
        }

        if (type == typeof(string)) {
            return (T)(object)str;
        }

        var parse = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, [typeof(string), typeof(IFormatProvider)], null);

        if (parse is not null && parse.ReturnType == type) {
            return (T?)parse.Invoke(null, [str, CultureInfo.InvariantCulture]);
        }

        parse = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, [typeof(string)], null);

        if (parse is not null && parse.ReturnType == type) {
            return (T?)parse.Invoke(null, [str]);
        }

        throw new NotSupportedException(
            "You must define a public static T Parse(string[, IFormatProvider]) method before invoking GetAttributeValueWithDefault<T>!");
    }

    public static void GetAttributeValueWithDefault<T>(
        this XElement element,
        out T? result,
        XName name,
        T? defaultValue = default) {
        if (typeof(T).IsSubclassOf(typeof(Enum))) {
            element.GetAttributeValueEnumWithDefault(out result, name, defaultValue);
        } else {
            result = element.GetAttributeValueWithDefault(name, defaultValue);
        }
    }

    public static void SetAttributeValueWithDefault<T>(this XElement element, XName name, T value, T? defaultValue = default) {
        if (!ReferenceEquals(value, defaultValue) && value is not null && !value.Equals(defaultValue)) {
            element.SetAttributeValue(name, value.ToString());
        }
    }

    private static bool IsNullable(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    private static T GetAttributeValueEnum<T>(this XElement element, XName name) => (T)Enum.Parse(typeof(T), element.GetAttributeValue(name)!, true);

    private static void GetAttributeValueEnum<T>(this XElement element, out T result, XName name)
        => result = (T)Enum.Parse(typeof(T), element.GetAttributeValue(name)!, true);

    private static T? GetAttributeValueEnumWithDefault<T>(this XElement element, XName name, T? defaultValue = default) {
        var str = element.GetAttributeValue(name);

        return string.IsNullOrWhiteSpace(str) ? defaultValue : (T)Enum.Parse(typeof(T), str, true);
    }

    private static void GetAttributeValueEnumWithDefault<T>(this XElement element, out T? result, XName name, T? defaultValue = default) {
        var str = element.GetAttributeValue(name);

        result = string.IsNullOrWhiteSpace(str) ? defaultValue : (T)Enum.Parse(typeof(T), str, true);
    }
}
