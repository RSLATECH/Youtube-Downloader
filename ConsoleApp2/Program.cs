using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.DemoConsole.Utils;
using YoutubeExplode.Videos;
string userName = Environment.UserName;
string videoId;

var youtube = new YoutubeClient();
while (true)
{
    Console.WriteLine("Welcome to youtube video downloader. Input the youtube video link or ID that you would like to download.");

    try
    { 
        videoId = VideoId.Parse(Console.ReadLine() ?? "");
        var video1 = await youtube.Videos.GetAsync(videoId);
        break;
    }
    catch (System.ArgumentException)
    {
        Console.WriteLine("That is an invalid video URL or ID, try again.");
    }

}

var video = await youtube.Videos.GetAsync(videoId);
var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
Console.WriteLine($"\nTitle: {video.Title}\nChannel: {video.Author.Title}\nDuration: {video.Duration}");
Console.WriteLine("\nWould you like MUXED, VIDEO only, or AUDIO only? (M/V/A) \n*Muxed video download is capped at 720p30.");

while (true)
{
    string readOnly1 = Console.ReadLine();

    if (readOnly1 == "M" || readOnly1 == "m")
    {
        var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
        Console.Write($"Downloading stream: {streamInfo.VideoQuality.Label} / {streamInfo.Container.Name}... ");

        var fileName = @$"C:\Users\{userName}\Downloads\{video.Title}.{streamInfo.Container.Name}";

        using (var progress = new InlineProgress())
            await youtube.Videos.Streams.DownloadAsync(streamInfo, fileName, progress);
        Console.WriteLine($"Video saved to '{fileName}'");
        break;
    }

    if (readOnly1 == "A" || readOnly1 == "a")
    {
        var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
        Console.Write($"Downloading stream: {streamInfo.Bitrate} / {streamInfo.Container.IsAudioOnly}... ");

        var fileName = @$"C:\Users\{userName}\Downloads\{video.Title}.mp3";

        using (var progress = new InlineProgress())
            await youtube.Videos.Streams.DownloadAsync(streamInfo, fileName, progress);

        Console.WriteLine($"Video saved to '{fileName}'");
        break;
    }

    if (readOnly1 == "V" || readOnly1 == "v")
    {
        var streamInfo = streamManifest
            .GetVideoOnlyStreams()
            .Where(s => s.Container == Container.Mp4)
            .GetWithHighestVideoQuality();
        Console.Write($"Downloading stream: {streamInfo.VideoQuality.Label} / {streamInfo.Container.Name}... ");

        var fileName = @$"C:\Users\{userName}\Downloads\{video.Title}.{streamInfo.Container.Name}";

        using (var progress = new InlineProgress())
            await youtube.Videos.Streams.DownloadAsync(streamInfo, fileName, progress);

        Console.WriteLine($"Video saved to '{fileName}'");
        break;
    }

    else
    {
        Console.WriteLine("That is not a valid input, please try again.");
        Console.WriteLine("\nWould you like MUXED, VIDEO only, or AUDIO only? (M/V/A) \n*Muxed video download is capped at 720p30\n");

    }

}

Console.WriteLine("Press any key to exit.");
Console.ReadLine();

