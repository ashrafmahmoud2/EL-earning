using System.IO;
using System.Reflection;


public class EmailBodyBuilder
{
    public static string GenerateEmailBody(string templateName, Dictionary<string, string> templateModel)
    {
        // Get the path to the directory where the assembly is located
        var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        // Build the path to the template
        var templatePath = Path.Combine(assemblyLocation, "Templates", templateName);

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Template not found: {templatePath}");
        }

        // Load the template and replace placeholders with values from templateModel
        var templateContent = File.ReadAllText(templatePath);
        foreach (var model in templateModel)
        {
            templateContent = templateContent.Replace($"{{{{{model.Key}}}}}", model.Value);
        }

        return templateContent;
    }
}
