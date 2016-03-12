# Tesseract box files creator and editor
This is very simple editor of tesseract box files for Windows, in which you can create those files from scratch and not just refine tesseract's output.

[Download binary releases here](https://github.com/EvilVir/tesseract-box-creator/releases)
[Grab source code here](https://github.com/EvilVir/tesseract-box-creator.git)

##For now you can:

1. Load any one-page image
2. Create new box file from scratch or load existing one
3. Draw directly on the image
4. Fine edit box coordinates on the grid view
5. Zoom in/out

##For now you can't:

1. Load multipage tiff (most of code is there but needs some pieces to do).
2. Train your tesseract from this editor - this is just boxes editor, nothing more for now but also much more than anything I could find on the Web.

And yes, I know that some parts of this code could/should be done better. For now I just need simple editor to kickstart my other project, refinements will maybe come later (or you can pull and do some ;).

##Used technologies:
.NET 4.6.1, C#, WPF
