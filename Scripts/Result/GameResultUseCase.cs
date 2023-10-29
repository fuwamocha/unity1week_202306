using MessagePipe;
namespace Game.Scripts.Result
{
    public class GameResultUseCase
    {
        private readonly IPublisher<ResultData> _onResult;

        public GameResultUseCase(IPublisher<ResultData> onResult)
        {
            _onResult = onResult;
        }

        public void In(ResultData resultData)
        {
            _onResult.Publish(resultData);
        }
    }
}
