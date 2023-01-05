public class D {
    public string Id {get; set;} = Guid.NewGuid().ToString().Replace("-","");
    public List<Ci> Cis {get; set;} = new List<Ci>();
}