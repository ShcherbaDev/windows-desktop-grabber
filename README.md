# Windows Desktop Grabber

This application gets data from Windows desktop and returns as XML

Currently tested on Windows 10 x64

## Run

Clone the repository and run `dotenv run` in your terminal

## Output

Application returns a XML document in the console with the following structure:

- root
  - platform - current platform (hardcoded to "windows")
  - wallpaper - desktop wallpaper and contains the following attributes:
    - when background is an image than returns these attributes:
      - path - absolute path to the wallpaper image
      - tile - should wallpaper be tiled or not (1 or 0)
      - style - position parameter which can contain the following values:
        - 0 - center
        - 2 - stretched fill the screen
        - 6 - resized to fit the screen while maintaining the aspect ratio (Windows 7 and later)
        - 10 - resized and cropped to fill the screen while maintaining the aspect ratio (Windows 7 and later)
    - when background is a solid color then returns these attributes:
      - rgb - RGB color
  - icons - array of icon elements
    - icon - text content is the desktop icon's name and contains the following attributes:
      - x - integer with horizontal position of the icon
      - y - integer with vertical position of the icon
      - size - width and height in pixels separated by comma

Sample output looks like this:

```xml
<?xml version="1.0"?>
<root xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <platform>windows</platform>
  <wallpaper path="c:\windows\web\wallpaper\theme1\img13" tile="0" style="10" />
  <icons>
    <icon x="17" y="2" size="95,65">This PC</icon>   
    <icon x="17" y="127" size="95,65">Firefox</icon>
    <icon x="1632" y="502" size="95,65">Random file.txt</icon>
    <icon x="1822" y="877" size="95,65">Recycle Bin</icon>
  </icons>
</root>
```

## TODO

- Get actual images from icons
- Get more accurate icon sizes
