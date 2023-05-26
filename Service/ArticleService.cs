namespace Moments.Service;

public class ArticleService
{
    private readonly IFreeSql _db;

    public ArticleService(IFreeSql db)
    {
        _db = db;
    }

    

}