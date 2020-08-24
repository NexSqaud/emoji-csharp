# emoji-csharp

Ported emoji library from [iris:emoji-java](https://github.com/iris2iris/iris-emoji-java)

## How to get it?

Via NuGet:

- Manage NuGet Packages: Search and install EmojiCSharp
- Package Manager: Install-Package EmojiCSharp -Version 1.0.0
- .NET CLI: dotnet add package EmojiCSharp --version 1.0.0
- PackageReference: `<PackageReference Include="EmojiCSharp" Version="1.0.0" />`

## How to use it?

### EmojiManager

The `EmojiManager` provides several static methods to search through the emojis database:

- `Init` initialize a database, run it once in `Main`
- `GetForTag` returns all the emojis for a given tag
- `GetForAlias` returns the emoji for an alias
- `GetAll` returns all the emojis
- `IsEmoji` checks if a string is an emoji
- `ContainsEmoji` checks if a string contains any emoji

You can also query the metadata:

- `GetAllTags` returns the available tags

Or get everything:

- `GetAll` returns all the emojis

### Emoji model

- `Unicode` the unicode respresentation of the emoji
- `GetUnicode(Fitzpatrick)` returns the unicode representation of the emoji with the provided Fitzpatrick modifier. If the emoji doesn't support the Fitzpatrick modifiers, this method will throw an `InvalidOperationException`. If Fitzpatrick is invalid, this method will throw an `ArgumentException`
- `Description` the (optional) description of the emoji
- `Aliases` a list of aliases for this emoji
- `Tags` a list of tags for this emoji
- `HtmlDecimal` an html decimal representation of the emoji
- `HtmlHexadecimal` an html decimal representation of the emoji
- `SupportsFitzpatrick` true if the emoji supports the Fitzpatrick modifiers, else false

### Fitzpatrick modifiers

Some emojis now support the use of Fitzpatrick modifiers that gives the choice between 5 shades of skin tones:

| Modifier | Types            |
| :------: | ---------------- |
|    🏻     | type12, type_1_2 |
|    🏼     | type3, type_3    |
|    🏽     | type4, type_4    |
|    🏾     | type5, type_5    |
|    🏿     | type6, type_6    |

We defined the format of the aliases including a Fitzpatrick modifier as:

```
:ALIAS|TYPE:
```

A few examples:

```
:boy|type_1_2:
:fist|type12:
:swimmer|type4:
:santa|type_6:
```

### EmojiParser

#### To unicode

To replace all the aliases and the html representations found in a string by their unicode, use `EmojiParser#ParseToUnicode(String)`.

For example:

```csharp
string str = "An :grinning:awesome :smiley:string &#128516;with a few :wink:emojis!";
string result = EmojiParser.ParseToUnicode(str);
Console.WriteLine(result);
// Prints:
// "An 😀awesome 😃string 😄with a few 😉emojis!"
```

#### To aliases

To replace all the emoji's unicodes found in a string by their aliases, use `EmojiParser#ParseToAliases(String)`.

For example:

```csharp
string str = "An 😀awesome 😃string with a few 😉emojis!";
string result = EmojiParser.ParseToAliases(str);
Console.WriteLine(result);
// Prints:
// "An :grinning:awesome :smiley:string with a few :wink:emojis!"
```

By default, the aliases will parse and include any Fitzpatrick modifier that would be provided. If you want to remove or ignore the Fitzpatrick modifiers, use `EmojiParser#ParseToAliases(String, FitzpatrickAction)`. Examples:

```csharp
string str = "Here is a boy: \uD83D\uDC66\uD83C\uDFFF!";
Console.WriteLine(EmojiParser.ParseToAliases(str));
Console.WriteLine(EmojiParser.ParseToAliases(str, FitzpatrickAction.Parse));
// Prints twice: "Here is a boy: :boy|type_6:!"
Console.WriteLine(EmojiParser.ParseToAliases(str, FitzpatrickAction.Remove));
// Prints: "Here is a boy: :boy:!"
Console.WriteLine(EmojiParser.ParseToAliases(str, FitzpatrickAction.Ignore));
// Prints: "Here is a boy: :boy:🏿!"
```

#### To HTML

To replace all the emoji's unicodes found in a string by their html representation, use `EmojiParser#ParseToHtmlDecimal(String)` or `EmojiParser#ParseToHtmlHexadecimal(String)`.

For example:

```csharp
string str = "An 😀awesome 😃string with a few 😉emojis!";

string resultDecimal = EmojiParser.ParseToHtmlDecimal(str);
Console.WriteLine(resultDecimal);
// Prints:
// "An &#128512;awesome &#128515;string with a few &#128521;emojis!"

string resultHexadecimal = EmojiParser.ParseToHtmlHexadecemal(str);
Console.WriteLine(resultHexadecimal);
// Prints:
// "An &#x1f600;awesome &#x1f603;string with a few &#x1f609;emojis!"
```

By default, any Fitzpatrick modifier will be removed. If you want to ignore the Fitzpatrick modifiers, use `EmojiParser#ParseToAliases(String, FitzpatrickAction)`. Examples:

```csharp
string str = "Here is a boy: \uD83D\uDC66\uD83C\uDFFF!";
Console.WriteLine(EmojiParser.ParseToHtmlDecimal(str));
Console.WriteLine(EmojiParser.ParseToHtmlDecimal(str, FitzpatrickAction.Parse));
Console.WriteLine(EmojiParser.ParseToHtmlDecimal(str, FitzpatrickAction.Remove));
// Print 3 times: "Here is a boy: &#128102;!"
Console.WriteLine(EmojiParser.ParseToHtmlDecimal(str, FitzpatrickAction.Ignore));
// Prints: "Here is a boy: &#128102;🏿!"
```

The same applies for the methods `EmojiParser#ParseToHtmlHexadecemal(String)` and `EmojiParser#ParseToHtmlHexadecemal(String, FitzpatrickAction)`.

#### Remove emojis

You can easily remove emojis from a string using one of the following methods:

- `EmojiParser#RemoveAllEmojis(String)`: removes all the emojis from the string
- `EmojiParser#RemoveAllEmojisExcept(String, IEnumerable<Emoji>)`: removes all the emojis from the string, except the ones in the list
- `EmojiParser#RemoveEmojis(String, IEnumerable<Emoji>)`: removes the emojis in the list from the string

For example:

```csharp
string str = "An 😀awesome 😃string with a few 😉emojis!";
List<Emoji> list = new List<Emoji>();
list.Add(EmojiManager.GetForAlias("wink")); // This is 😉

Console.WriteLine(EmojiParser.RemoveAllEmojis(str));
Console.WriteLine(EmojiParser.RemoveAllEmojisExcept(str, list));
Console.WriteLine(EmojiParser.RemoveEmojis(str, list));

// Prints:
// "An awesome string with a few emojis!"
// "An awesome string with a few 😉emojis!"
// "An 😀awesome 😃string with a few emojis!"
```

#### Extract Emojis from a string

You can search a string of mixed emoji/non-emoji characters and have all of the emoji characters returned as a list.

- `EmojiParser#ExtractEmojis(String)`: returns all emojis as a list. This will include duplicates if emojis are present more than once.

## Credits

**[emoji-csharp](https://github.com/NexSqaud/emoji-csharp)** is based on [iris-emoji-java](https://github.com/iris2iris/iris-emoji-java)

**[iris-emoji-java](https://github.com/iris2iris/iris-emoji-java)** is based on [github/vdurmont/emoji-java](https://github.com/vdurmont/emoji-java).

And in its turn **emoji-java** originally used the data provided by the [github/gemoji project](https://github.com/github/gemoji). It is still based on it but has evolved since.

## Don't forget to give stars ⭐