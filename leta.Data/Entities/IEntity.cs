namespace leta.Data.Entities
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; }

    }
}
