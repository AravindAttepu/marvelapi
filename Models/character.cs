using System.Runtime.InteropServices.Marshalling;

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Thumbnail Thumbnail { get; set; }
    public Comics comics {get; set;}
}

//single serial
public class SeriesList{
    public int ID{get; set;}
    public string Title{get; set;}
    public int StartYear{get; set;}
    public int EndYear{get; set;}
    public string Rating{get; set;}
    public string Description{get; set;}
    public string ResourceUrl{get; set;}
    public Thumbnail  thumbnail{get; set;}
     
     public Characters characters{get; set;}
     public List<urls> Urls{get; set;}
     
 

    
}
public class CharacterSeries{
  
    public List<SeriesList> seriesLists{get; set;}

}
public class CharacterComics{
public List<comicitem> comics{get; set;}
}

//single comic
public class comicitem 
{
    public string ResourceUrl{get; set;}
    public int ID{get; set;}
    public string Title {get; set;}
    public string Description { get; set; }
    public Thumbnail thumbnail{get; set;}
    public List<Character> character{get; set;}
     public Characters Characters{get; set;}
     public List<urls> Urls{get; set;}
     public List<Thumbnail> Images{get; set;}
     public List<textObjects> textObjects{get; set;}
}

public class Characters{

    int available{get; set;}
    public List<charcterslist> items{get; set;}
}
public class urls{
    public string  type{get; set;}
    public string url{get; set;}
}

public class charcterslist{
    public string Name{get; set;}
}

public class textObjects{
    public string Text{get; set;}
}

public class CharComicModel{
    public Character character{get; set;}
    public List<comicitem> comics{get; set;}
    public List<SeriesList> series{get; set;}
}

public class Comics{
    public int available{get; set;}

    public List<comicitem> Items{get; set;}
}



public class Thumbnail
{
    public string Path { get; set; }
    public string Extension { get; set; }

    public string FullPath => $"{Path}.{Extension}";
}

public class MarvelApiResponse<T>
{
    public DataContainer<T> Data { get; set; }
}

public class DataContainer<T>
{
    public int Offset { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }
    public int Count { get; set; }
    public List<T> Results { get; set; }
}
public class MarvelCharacterResponse<T> 
{
   public DataContainer2<T> Data { get; set; } 
}
public class DataContainer2<T>
{
    public int Offset { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }
    public int Count { get; set; }
   
    public Character Results { get; set; }
}