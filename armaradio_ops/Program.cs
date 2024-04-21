// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

//var processor = new armaradio_ops.Operations.Processor();
//processor.StartParsingArtistNames();  // insert artist names

//var albumsProcessor = new armaradio_ops.Operations.AlbumsProcessor();
//albumsProcessor.ProcessAlbums();

//var audioDbArtistsProcessor = new armaradio_ops.Operations.AudioDbProcessor();
//audioDbArtistsProcessor.SaveArtistsFromJsonFile();

var mbAlbumsProcessor = new armaradio_ops.Operations.MBAlbumProcessor();
//mbAlbumsProcessor.SplitLargeJsonFile();
mbAlbumsProcessor.ProcessAlbumsFromJsonFile();

