# TEdit plugin for pixel-ART

To add these plugins to your TEDit, you need:
1. Download the open source code of tedit and place the PixelArtTools folder in the plugins folder.
2. In the ViewModelLocator.cs file, in the CreateWorldViewModel function, add the following line of code:
```csharp
            //Sorted by Plugin-Name
            wvm.Plugins.Add(new PixelArtCreator(wvm)); // my
  ```
3. Then the application can be builded (ctr + b).
