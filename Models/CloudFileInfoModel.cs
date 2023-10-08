namespace YaDiskObserver.Models;

public class CloudFileInfoModel
{
    public string path { get; set; }
    public string type { get; set; }
    public string name { get; set; }
    public DateTime modified { get; set; }
    public DateTime created { get; set; }
    public string preview { get; set; }
    public string md5 { get; set; }
    public string mime_type { get; set; }
    public int? size { get; set; } 
}