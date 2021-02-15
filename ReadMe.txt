J'ai donc repris mon projet de 3ème année pour y intégrer des fonctionnalités de XtraLife, j'ai enlevé quelques plugins/shaders qui encombrent un peu l'espace pour ce genre de test.

La majorité des changements que j'ai fais se trouvent dans Assets\Project\01_Scripts\XtraLifeManager et dans Assets\Project\01_Scripts\UIManager.

J'ai rajouté un écran pour se login/créer un compte au démarrage, un bouton pour se reconnecter à la session d'avant et un bouton pour se logout quand on fait pause.
Je check si l'utilisateur existe dèjà sur le serveur lors du login, si c'est le cas, je récupère ses variables utilisateurs, sinon je les crées à ce moment là. 

Je stock 3 variables par joueur, si celui-ci a dèjà vu la séquence d'introduction ( pour lui offrir la possibilité de la passer en pressant 'F' le cas échéant, je check aussi
cette variable pour montrer ou non le panel d'inputs au lancement du jeu). Je stock aussi son meilleur temps et son meilleur score. 

Lorsque le joueur franchit la porte au fond de l'arène cela veut dire qu'il a fini l'épreuve, je check à ce moment là pour voir s'il a un meilleur score qu'avant, si son score
( temps ou score ) fait parti du top 5 je l'affiche dans le leaderboard, que j'affiche ensuite. 

Il y a 2 inputs "dev" pour tester plus rapidement :

- "0" sur le clavier de droite, pour delete les keys de playerPrefs

- "6" sur le clavier de droite, ouvre la porte du fond de l'arène quand on se trouve dans celle-ci. 