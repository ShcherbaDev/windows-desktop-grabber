# Windows Desktop Grabber

This application gets data from Windows user's desktop and returns as XML.

Currently tested on Windows 10 21H2 x64.

Target framework is `net5.0-windows`.

## Parameters

| Name                     | Type    | Required | Default   | Description |
|:-------------------------|:--------|:---------|:----------|:------------|
| --ignore-virtual-folders | Boolean | No       | false     | Ignore virtual folders like \"My PC\", \"Recycle Bin\" etc. |
| --exclude-icon-images    | Boolean | No       | false     | Should not add actual image of the icon |
| --exclude-wallpaper      | Boolean | No       | false     | Should not add wallpaper data to the output |
| --icon-images-path       | String  | No       | `./icons` | Relative path to the directory with icon's images |

## Run

Clone the repository and run `dotnet run` in your terminal

## Build

Run this following command to build:

`dotnet build -c Release`

And then run windows-desktop-grabber.exe in `bin/Release/net5.0-windows`

## Output

Application returns a XML document in the console with the following structure:

- root
  - platform - current platform (hardcoded to "windows")
  - icon-images-path - relative path to the directory with icon's images
  - wallpaper - desktop wallpaper and contains the following attributes:
    - path - absolute path to the wallpaper image (is present)
    - rgb - RGB background when image is not present or is not fitting on screen (numbers are divided by space)
    - tile - should wallpaper be tiled or not (1 or 0)
    - style - position parameter which may contain the following values:
      - 0 - center
      - 2 - stretched fill the screen
      - 6 - resized to fit the screen while maintaining the aspect ratio (Windows 7 and later)
      - 10 - resized and cropped to fill the screen while maintaining the aspect ratio (Windows 7 and later)
  - icons - array of icon elements
    - icon - text content is the desktop icon's name and contains the following attributes:
      - x - integer with horizontal position of the icon
      - y - integer with vertical position of the icon
	  - type - enum parameter which may contain the following values:
	  	- 0 - file
		- 1 - directory
		- 2 - shortcut
		- 3 - system (e.g. Recycle bin, This PC)
      - size - width and height in pixels separated by comma
	  - image-filename - icon's actual image filename without extension (hidden if parameter `--exclude-icon-images` is enabled)

Sample output looks like this:

```xml
<?xml version="1.0"?>
<root xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <platform>windows</platform>
  <icon-images-path>./icons</icon-images-path>
  <wallpaper path="c:\windows\web\wallpaper\theme1\img13" tile="0" style="10" />
  <icons>
    <icon x="17" y="2" type="3" size="95,65" image-filename="370b02a0-63f0-4642-b246-fb76ca3324e4">This PC</icon>   
    <icon x="17" y="127" type="2" size="95,65" image-filename="d25a3ed3-e4c9-4c9b-a8b1-3cf6d3dc7851">Firefox</icon>
    <icon x="1632" y="502" type="0" size="95,65" image-filename="346e79b3-43a2-42a5-9365-875b8e7ff834">Random file.txt</icon>
    <icon x="1822" y="877" type="3" size="95,65">Trash Bin</icon>
  </icons>
</root>
```

If the user's desktop contains any shortcuts and the `--exclude-icon-images` parameter is disabled then the `shortcut_overlay.png` file will be created. This is a system shortcut arrow overlay image.

## TODO

- Get system images from icons like "Recycle Bin" or "This PC"
