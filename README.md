# Snow-Displacement-Unity6

-- [ENGLISH WILL FOLLOW](#english-version) --

Projet personnel de traînée de neige pour apprendre à utiliser les shaders dans Unity 6 et pour me familiariser avec les compute shaders.

### Fonctionnement

#### Compute Shader
À la base du projet est le compute shader "SnowTrailCompute.compute". Celui-ci dessine en noir sur une texture blanche prédéfinie la position actuelle du joueur. L'ajout à chaque frame de cette position résulte en une traînée noire sur la texture.


#### Tessellation
Afin d'assurer la bonne définition de la traînée sur le mesh de l'objet, une étape de tessellation a été ajoutée dans le fichier "SnowTessellation.hlsl. Un facteur de tessellation est exposé dans l'inspecteur Unity pour mieux contrôler le niveau de tessellation et donc la résolution de la traînée.


#### Unlit Shader
Le tout est finalement assemblé dans le fichier "SnowUnlitShader.shader". Dans le vertex shader, la texture dessinée par le compute shader est utilisé pour définir la hauteur du vertex. Une multiplication de la couleur de la texture (0 si noir et 1 si blanc) par la hauteur originale du vertex permet de créer la traînée dans le mesh.

D'autre paramètres sont présents dans le shader afin de personnaliser facilement le rendu final:

- Tessellation factor : Le facteur de subdivision de l'étape de tessellation. Plus le chiffre est élevé, plus il y aura de subdivisions.
- Base snow height : Permet de définir une hauteur de base pour le mesh de neige.
- Snow height variation : Permet l'ajout d'une texture de bruit pour ajouter une variation à la hauteur de base de la neige.
- Snow height variation weight : Poids de la texture "Snow height variation". Autrement dit, c'est à quel point la texture influence la hauteur de base.
- Snow texture : Permet l'ajout d'une texture pour le visuel de la neige (blanc par défaut).


### Utilisation
1. Ajouter le composant "SnowTrailController" au game object qui représente le sol enneigé.
2. S'assurer que le matériel de ce game object utilise le shader "Custom/SnowUnlitShader".
3. Ajouter les éléments nécessaires dans l'inspecteur du composant "SnowTrailController"
    1. Le compute shader
    2. La texture de traînée
    3. Le mesh du sol
    4. Le mesh de l'élément qui fait la traînée
4. Ajuster les propriétés de la traînée dans l'inspecteur du composant
    1. La grosseur de la traînée
    2. La quantité de "neige" qui re-remplit la traînée à chaque itération de remplissage
    3. Le temps (en secondes) entre chaque itération de remplissage de la traînée
5. Ajuster les propriété dans le shader (voir section [UnlitShader](#unlit-shader)).
6. Appeler `SnowTrailController.Instance.DrawSnowTrail(Vector3)` dans le script contrôlant le joueur. C'est cette méthode qui appelle le compute shader pour dessiner la texture de traînée.

Voilà ! Il ne reste qu'à cliquer le bouton play et observer la traînée qui est créée dans le sol.

***
<a name="english-version" /> -- ENGLISH VERSION --

This is a personal project of a snow trailing system made to learn shaders in Unity 6 and to familiarize myself with compute shaders.

### How it works

#### Compute shader
At the root of everything is the compute shader "SnowTrailCompute.compute". It's used to draw a black circle representing the player current position on a predetemined white texture. The addition of those positions creates a black trail on the base texture.


#### Tessellation
To make sure the trail is well defined on the object's mesh, a tessellation step was added in "SnowTessellation.hlsl". A tessellation factor is exposed in the Unity inspector to allow the user to better control the tessellation level, and in doing so, the trail resolution.


#### Unlit Shader
Everything is then assembled in "SnowUnlitShader.shader". In the vertex shader, the texture that was drawn on by the compute shader is used to define the vertex height. Creating the trail is as simple as multiplying the texture color (0 if black and 1 if white) by the vertex's original height.

Parameters are present in the shader to facilitate personalization of the trail final render:

- Tessellation factor: The tessellation step subdivision factor, the bigger the number, the more subdivisions are made in the tessellation step.
- Base snow height: Used to set a base height for the snow mesh.
- Snow height variation: Allows the use of a noise texture to add variations to the base height of the snow mesh.
- Snow height variation weight: Specifies how much the noise texture influences the base height.
- Snow texture: Allows the use of a texture to change the look of the snow (white by default).


### How to use it
1. Add the "SnowTrailController" component to the game object that is gonna serve as your snowy ground.
2. Make sure that the snowy ground game object's material uses the "Custom/SnowUnlitShader" shader.
3. Add the necessary elements to the "SnowTrailController" component in the Unity inspector.
    1. The compute shader
    2. Texture used for the trail
    3. Snowy ground's mesh
    4. Mesh of the entity make the trail in the snow
4. Tweak the properties of the component in the Unity inspector.
    1. Trail's size
    2. How much snow is added at each snow filling iteration
    3. How much time (in seconds) in-between each snow filling iteration
5. Tweak the properties in the Shader (see [Unlit Shader](#unlit-shader-1) section)
6. Call `SnowTrailController.Instance.DrawSnowTrail(Vector3)`in the script that controls the entity making the trail. Its the function that calls the compute shader and draw the trail on the texture.

And there it is! All thats left is to click the Play button and enjoy the trail that your player leaves in the snow's mesh.
