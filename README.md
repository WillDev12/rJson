<p align="center"><img src="json-4.png" width="200" height="200"></p>

<h1 align="center">rJson</h1>

<p align="center">
<img src="https://img.shields.io/github/followers/WillDev12?label=follow%20my%20github&logo=github&style=for-the-badge">
<img src="https://img.shields.io/github/stars/WillDev12/rJson?style=for-the-badge">
<img src="https://img.shields.io/github/forks/WillDev12/rJson?style=for-the-badge">
<img src="https://img.shields.io/github/watchers/WillDev12/rJson?style=for-the-badge">
</p>

<details>
<summary>What is this?</summary>
<br>
rJson is a powerful VS C# Library that allows you to pass encrypted data to and from files.
<br>
</details>

<details>
<summary>How to use</summary>
<br>
To start, simply prime with the following:

``` c#
using rJson;

rJs rJs = new rJs;
```

You can then transfer variables like so:

``` c#
var yourStuff = rJs.New("value1=hello,value2=world,password=123", null); 
// If you would like to send your data to a file, simply write the filepath
// in place for null.
```

Now, parse and grab some data!
``` c#
var parsed = rJs.parse(yourStuff, null);
// Here you can leave one or the other null.  As for the first parameter,
// that is for if you want to parse in-program rJson as shown, otherwise,
// swap the two variables but instead replace `yourStuff` with a filepath.

Console.WriteLine(rJs.getVar(parsed, "value1")); // will return "hello"
```
</details>

<details>
<summary>Owner</summary>
<br>

```
WillDev12 (WillDevv12)
```
</details>

<details>
<summary>License</summary>
<br>

```
MIT License
```
This project is open source and free to edit.

</details>

<a href="https://www.nuget.org/packages/rJson">
    <img src="http://md-announcements.vercel.app/api?type=horizontal&theme=yellow&text=Be%20sure%20to%20check%20out%20our%20Nuget%20page!&background=true" style="width: 100%;"></img>
</a>

# Like content like this?

Check out my [profile](https://github.com/WillDev12) or share it! (this means a lot to me)

```
https://github.com/WillDev12
https://WillDev12.github.io
```
