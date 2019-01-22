namespace HelloServerless
{
    public class ResultStatus<T>
    {
        public bool Status { get; set; }
        public T Data { get; set; }

        public static ResultStatus<T> Success(T data)
        {
            return new ResultStatus<T>
            {
                Status = true,
                Data = data
            };
        }

        public static ResultStatus<T> Error()
        {
            return new ResultStatus<T>
            {
                Status = false
            };
        }
    }
}