namespace SvcCommon.Abstract;

public interface IConcurrencyAware
{
    string ConcurrencyStamp { get; set; }
}