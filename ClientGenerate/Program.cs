using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using NSwag.CodeGeneration.TypeScript;

var document = await OpenApiDocument.FromUrlAsync("https://localhost:7047/swagger/v1/swagger.json");

var settings = new TypeScriptClientGeneratorSettings
{
    ClassName = "{controller}Client",
    Template = TypeScriptTemplate.Fetch,
    TypeScriptGeneratorSettings = {TypeStyle = TypeScriptTypeStyle.Interface},
};

var generator = new TypeScriptClientGenerator(document, settings);
var code = generator.GenerateFile();

await File.WriteAllTextAsync(
    Path.Join(Directory.GetCurrentDirectory(), "..", "..", "client", "lib", "api", "src", "api-client.ts"),
    code
);