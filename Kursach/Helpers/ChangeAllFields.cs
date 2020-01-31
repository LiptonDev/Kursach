namespace Kursach
{
    /// <summary>
    /// Магия рефлексии.
    /// </summary>
    static class ChangeAllFields
    {
        public static void SetAllFields<T>(this T source, T newObject)
        {
            if (source == null || newObject == null)
                return;

            var props = source.GetType().GetProperties();
            var newProps = newObject.GetType().GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                var currentProp = props[i];
                var newProp = newProps[i];

                if (currentProp.GetCustomAttributes(typeof(ChangeIgnoreAttribute), true).Length == 1)
                    continue;

                currentProp.SetValue(source, newProp.GetValue(newObject));
            }
        }
    }
}
