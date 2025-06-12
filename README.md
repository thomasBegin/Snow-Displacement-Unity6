# Snow-Displacement-Unity6
-- ENGLISH WILL FOLLOW --

Projet personnel de traînée de neige pour apprendre à utiliser les shaders dans Unity 6 et pour me familiariser avec les compute shaders.

### Fonctionnement

#### Compute Shader
À la base du projet est le compute shader "SnowTrailCompute.compute". Celui-ci dessine en noir sur une texture blanche prédéfinie la position actuelle. L'ajout à chaque frame de la position actuelle du joueur résulte en une traînée noire sur la texture.


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
5. Ajuster les propriété dans le shader (voir section UnlitShader).
6. Appeler `SnowTrailController.Instance.DrawSnowTrail(Vector3)` dans le script contrôlant le joueur. C'est cette méthode qui appelle le compute shader pour dessiner la texture de traînée.

Voilà ! Il ne reste qu'à cliquer le bouton play et observer la traînée qui est créée dans le sol.


