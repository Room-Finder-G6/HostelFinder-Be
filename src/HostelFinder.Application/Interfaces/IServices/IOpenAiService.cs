using HostelFinder.Application.DTOs.ChatAI.Request;
using HostelFinder.Application.DTOs.ChatAI.Response;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IOpenAiService
{
    Task<OpenAiChatResponse> GenerateAsync (OpenAiChatRequest request);
}