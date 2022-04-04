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

Sample output looks like this:

```xml
<?xml version="1.0"?>
<root xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <platform>windows</platform>
  <icons>
    <icon x="17" y="2">This PC</icon>   
    <icon x="17" y="127">Firefox</icon>
    <icon x="1632" y="502">Random file.txt</icon>
    <icon x="1822" y="877">Recycle Bin</icon>
  </icons>
</root>
```

## TODO

- Get actual images from icons
- Get background image and its properties
