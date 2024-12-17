var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Text("""
                                   <html>
                                    <body>
                                        <p id="theparagraph">Hello, World!</p>
                                    </body>
                                   </html>
                                   """, "text/html"));

await app.RunAsync();
