namespace CleanMicroserviceSystem.DataStructure;

public class CommonResult
{
    public CommonResult()
        : this(new List<CommonResultError>())
    {
    }

    public CommonResult(IList<CommonResultError> errors)
    {
        this.Errors = errors;
    }

    public bool Succeeded => !(this.Errors?.Any() ?? false);

    public IList<CommonResultError> Errors { get; set; }

    public override string ToString()
    {
        return this.Succeeded ?
            $"{nameof(this.Succeeded)}:{this.Succeeded}." :
            $"{nameof(this.Succeeded)}:{this.Succeeded}. {nameof(this.Errors)}:{string.Join("; ", (this.Errors ?? Enumerable.Empty<CommonResultError>()).Select(error => error.ToString()))}";
    }
}

public class CommonResult<T> : CommonResult
{
    public CommonResult()
        : base()
    {
    }

    public CommonResult(IList<CommonResultError> errors)
        : base(errors)
    {
    }

    public CommonResult(T? entity)
        : this()
    {
        this.Entity = entity;
    }

    public CommonResult(T? entity, IList<CommonResultError> errors)
        : this(errors)
    {
        this.Entity = entity;
    }

    public T? Entity { get; set; }

    public override string ToString()
    {
        return this.Succeeded ?
            $"{nameof(this.Succeeded)}:{this.Succeeded}. {nameof(this.Equals)}:{this.Entity?.ToString()}" :
            $"{nameof(this.Succeeded)}:{this.Succeeded}. {nameof(this.Errors)}:{string.Join("; ", (this.Errors ?? Enumerable.Empty<CommonResultError>()).Select(error => error.ToString()))}";
    }
}