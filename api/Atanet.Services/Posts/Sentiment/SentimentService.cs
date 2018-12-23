namespace Atanet.Services.Posts.Sentiment
{
    using System.Net.Http;
    using System;
    using System.Net;
    using ApiResult;
    using Exceptions;
    using Model.Dto.Sentiment;
    using Newtonsoft.Json;

    public class SentimentService : ISentimentService
    {
        private readonly string host = Environment.GetEnvironmentVariable("SENTIMENT_HOST");

        private readonly string port = Environment.GetEnvironmentVariable("SENTIMENT_PORT");

        private readonly IApiResultService apiResultService;

        public SentimentService(IApiResultService apiResultService)
        {
            this.apiResultService = apiResultService;
        }
        
        public float GetSentiment(string sentence)
        {
            var httpClient = new HttpClient();
            var sentimentResult = httpClient.GetAsync($"http://{host}:{port}?text={sentence}").Result;
            var textResponse = sentimentResult.Content.ReadAsStringAsync().Result;
            switch (sentimentResult.StatusCode)
            {
                case HttpStatusCode.OK:
                    var model = JsonConvert.DeserializeObject<SentimentModel>(textResponse);
                    return model.Sentiment;
                
                case HttpStatusCode.BadRequest:
                    var errorResponse = JsonConvert.DeserializeObject<SentimentAnalysisError>(textResponse);
                    throw new ApiException(this.apiResultService.BadRequestResult(errorResponse.Message));
                
                case HttpStatusCode.InternalServerError:
                    throw new ApiException(this.apiResultService.InternalServerErrorResult(null));
            }
            
            throw new ApiException(this.apiResultService.InternalServerErrorResult(null));
        }
    }
}