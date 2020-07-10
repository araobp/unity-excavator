# Pascal VOC XML auto-generation


[LabelImg](https://github.com/tzutalin/labelImg) is an annotation tool for training AI object detection models.

I use it for training SSD-MobileNet v2 model with my original images. LabelImg outputs annotation files in the format of Pascal VOC XML.

This project is to auto-generate annotation files for scenes on Untiy.

## Generating Pascal VOC XML

Just press "B" to generate a Pascal VOC XML file. The generated file will be saved in "Capture" folder.

![pascalvoc](./pascalvoc.png)

## Loading the XML into LabelImg

![labelimg](./labelimg.png)

## Code
- [=> Code on Unity](../PascalVOC)
