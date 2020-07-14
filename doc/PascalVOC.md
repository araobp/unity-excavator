# Pascal VOC XML auto-generation


[LabelImg](https://github.com/tzutalin/labelImg) is an annotation tool for training AI object detection models.

I use it for training SSD-MobileNet v2 model with my original images. LabelImg outputs annotation files in the format of Pascal VOC XML.

This project is to auto-generate annotation files for scenes on Untiy, **without using LabelImg**.

## Generating Pascal VOC XML

Just press "B" to generate a Pascal VOC XML file. The generated file will be saved in "Capture" folder.

![pascalvoc](./pascalvoc.png)

## Loading the XML into LabelImg

![labelimg](./labelimg.png)

## How it works

First, add a tag startigng with "name:" to target objects. For example, "name:ethan" or "name:car".

Then, this implementation employes the following technique to generate Pascal VOC XML files:
- It uses a Unity's Camera object as a depth camera to extract target objects only. 
- All the objects, that are not necessary to extract images, are deactivated.
- Data about a bounding box of the extracted image is then translated into Pascal VOC XML format.
- Those deactivated objects are re-activated just before capturing the scene as an image in JPEG format.

Those XML files and JPEG files are saved in "Assets/Capture" folder.

Note: Objects having a tag "Studio" is a set of objects that works like a studio with a green screen behind a target object.

## Code
- [=> Code on Unity](../PascalVOC)
