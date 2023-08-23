using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaBot.Application.Common.Models.Settings;
public class OpenAPISettings
{
    public string? EmbeddingsBaseUrl { get; set; }
    public string? ChatBaseUrl { get; set; }
    public string? APIKey { get; set; }
    public string? EmbeddingModel { get; set; }
    public string? ChatModel { get; set; }
}
