# Windows Desktop Grabber

This application gets data from Windows desktop and returns as XML

Currently tested on Windows 10 x64

## Run

Clone the repository and run `dotenv run` in your terminal

## Output

Application returns a XML document in the console with the following structure:

- root
  - platform - current platform (hardcoded to "windows")
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
- Get background image and its properties
