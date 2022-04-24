namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WordCountController : ControllerBase
{
    [HttpPost]
    public IActionResult Count(IFormFile file, int top = 10)
    {
        var stream = file.OpenReadStream();
        var sr = new StreamReader(stream);

        var wordCounts = new Dictionary<string, int>();
        string currentWord = string.Empty;

        while (sr.Peek() >= 0)
        {
            var c = (char)sr.Read();
            // if HTML, strip tags
            if (c == '<') // tag
            {
                if (!string.IsNullOrWhiteSpace(currentWord))
                {
                    if (!wordCounts.TryAdd(currentWord, 1))
                    {
                        ++wordCounts[currentWord];
                    }
                    currentWord = string.Empty;
                }

                string tag = string.Empty;

                // get tag
                while (sr.Peek() >= 0)
                {
                    var tc = (char)sr.Read();

                    if (char.IsWhiteSpace(tc) || tc == '!' || tc == '>')
                    {
                        break;
                    }

                    tag += tc;
                }

                // ignore everything between opening and closing script and style tags
                if (tag == "script" || tag == "style")
                {
                    while (sr.Peek() >= 0)
                    {
                        var tagChar = (char)sr.Read();

                        if (tagChar == '>')
                            break;  // throw away everything until closing carrot
                    }
                }

                while (sr.Peek() >= 0)
                {
                    var tagChar = (char)sr.Read();

                    if (tagChar == '>')
                        break;  // throw away everything until closing carrot
                }
                continue;
            }

            // ignore escaped chars
            if (c == '&')
            {
                string escape = string.Empty;
                while (sr.Peek() >= 0)
                {
                    var next = (char)sr.Read();
                    escape += next;

                    if (next == ';')
                    {
                        break; // throw it away
                    }
                }
            }

            if (char.IsSeparator(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c))
            {
                if (!string.IsNullOrWhiteSpace(currentWord))
                {
                    if (!wordCounts.TryAdd(currentWord, 1))
                    {
                        ++wordCounts[currentWord];
                    }
                }

                currentWord = string.Empty;
                continue;
            }

            currentWord += c;
        }

        return Ok(new { counts = wordCounts.OrderByDescending(x => x.Value).Take(top) });
    }
}
