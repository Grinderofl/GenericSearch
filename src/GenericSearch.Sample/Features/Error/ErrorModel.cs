namespace GenericSearch.Sample.Features.Error
{
    public class ErrorModel
    {
        public ErrorModel(string requestId)
        {
            RequestId = requestId;
        }

        public string RequestId { get; }

        public bool ShowRequestId => !string.IsNullOrWhiteSpace(RequestId);
    }
}