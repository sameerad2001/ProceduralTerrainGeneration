### Procedural terrain generation

This project creates a procedural terrain using perlin noise and it does so in the following manner :

1. A grid of evenly spaced vertices is generated
2. The height of each vertex is varied using unity's built in perlin noise method : `Mathf.PerlinNoise()`
3. The color of each vertex is calculated using the normalized height value
4. Triangles are generated to draw the mesh
5. The mesh is updated with the generated data 

![Demo](https://github.com/sameerad2001/ProceduralTerrainGeneration/blob/master/Demo/DemoImage.png)
