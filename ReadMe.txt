J'ai donc repris mon projet de 3ème année pour y intégrer des fonctionnalités de XtraLife.

La majorité des changements que j'ai fais se trouvent dans Assets\Project\01_Scripts\XtraLifeManager et dans Assets\Project\01_Scripts\UIManager

J'ai rajouté un écran pour se login/créer un compte au démarrage, pour se reconnecter à la session d'avant et un bouton pour se logout quand on fait pause.

Je stock 3 variables par joueur, si celui-ci a dèjà vu la séquence d'introduction ( pour lui offir la possibilité de la passer le cas échéant, je check aussi
cette variable pour montrer ou non le panel d'inputs au lancement du jeu ). Son meilleur temps et son meilleur score. 

Lorsque le joueur franchit la porte au fond de l'arène cela veut dire qu'il a fini l'épreuve, je check à ce moment là pour voir si il a un meilleur score qu'avant, 
je montre ensuite le leaderboard de score et celui de temps. 