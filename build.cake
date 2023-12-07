var target = Argument("target", "Default");
var env = Argument("env", "Development");

#addin "nuget:?package=Cake.Docker&version=1.2.2"

Setup(context =>
{
    if (target != "Default")
    {
        Information($"Running target {target} for environment {env}.");
    }
});


Task("Default")
    .IsDependentOn("Database:Run");

Task("Database:Run").Does(() =>
{
    DockerRunPostgres("wingrid-auth", "wingrid-auth", port: 5432);
    DockerRunPostgres("wingrid-events", "wingrid-events", port: 5433);
    DockerRunPostgres("wingrid-fixtures", "wingrid-fixtures", port: 5434);
});

private void DockerRunPostgres(string appName, string databaseName, int port = 5432, string username = "postgres", string password = "passW0rd")
{
    if (string.IsNullOrWhiteSpace(appName))
        throw new Exception("DockerRunPostgres Error: appName is required.");
    if (string.IsNullOrWhiteSpace(databaseName))
        throw new Exception("DockerRunPostgres Error: databaseName is required.");
    if (port < 0)
        throw new Exception("DockerRunPostgres Error: port must be greater than 0.");
    if (string.IsNullOrWhiteSpace(username))
        throw new Exception("DockerRunPostgres Error: username is required.");
    if (string.IsNullOrWhiteSpace(password))
        throw new Exception("DockerRunPostgres Error: password is required.");

    var settings = new DockerContainerRunSettings
    {
        Name = $"{appName}-db",
        Env = new[] { $"POSTGRES_USER={username}", $"POSTGRES_PASSWORD={password}", $"POSTGRES_DB={databaseName}" },
        Publish = new[] { $"{port}:5432" },
        Volume = new[] { $"{appName}-data:/var/lib/postgresql/data" },
        Detach = true
    };

    DockerRunWithoutResult(settings, $"postgres:latest", null);
    Information($"Postgres is starting up. Connect at localhost,{port}");
}

RunTarget(target);