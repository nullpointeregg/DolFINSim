   M S B A M L   `   `   `  ���� L  HDolFINSim_junuver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:DolFINSim_junuverDolFINSim_junuver  "    DolFINSim_junuver.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:DolFINSim_junuver       � Title$  
MainWindow��$	��800��$	��900�� � WindowStartupLocation$ CenterScreen=�.��� -    �Name$ BackGrid��.+�� -   $ FormGrid�� � ColumnDefinitions ��  � RowDefinitions �� $��25 ��� $��* �.+�� -   $ ToolGrid�� �� $��60 ��� $��60 ��� $��60 ��� $��* �.+��� -    ��Name$ 
FileButton��$
��File��$��15(�$
��Bold"�$��Stretch=�$��Stretch=����#FFFFFF$��0q�$��0��$��0���� -   $ OptionButton��$��Option��$��15(�$
��Bold"�$��Stretch=�$��Stretch=����#FFFFFF$��0q�$��0��$��1���� -   $ 
HelpButton��$
��Help��$��15(�$
��Bold"�$��Stretch=�$��Stretch=����#FFFFFF$��0q�$��0��$��2��� -   $ MainGrid��$��1�� �� $��* ��� $	��300 �.+�� -   $ DisplayGrid�����#9e9e9e$��0�� ��  �� $��* ��� $	��250 �.+��� -	    ��Name$ 
MainCanvas��$��0��� $��1�� �� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* �.+��� $��0�����#888888�  �� �� .+��� -
    ��Name$ Player1Counter��$��0��$��Times New Roman)�$��30(�$��Center=�$��Center=�
���   �
Player1 DS�� -   $ Player2Counter��$��1��$��Times New Roman)�$��30(�$��Center=�$��Center=�
�������
Player2 DS� -   $ DirectionGrid��$��1�� �� �� �� �� �� �� .+��� -   $ FirstMoveButton��$��First��$��30(�$
��Bold"�$��Stretch=�$��Stretch=�$��0���� -   $ Backward10Button��$��<<��$��30(�$
��Bold"�$��Stretch=�$��Stretch=�$��1���� -   $ BackwardButton��$��<��$��30(�$
��Bold"�$��Stretch=�$��Stretch=�$��2���� -   $ ForwardButton��$��>��$��30(�$
��Bold"�$��Stretch=�$��Stretch=�$��3���� -   $ Forward10Button��$��>>��$��30(�$
��Bold"�$��Stretch=�$��Stretch=�$��4���� -   $ LastMoveButton��$
��Last��$��30(�$
��Bold"�$��Stretch=�$��Stretch=�$��6���� $��
Your Turn:��$��Times New Roman)�$��40(�$
��Bold"�$
��Left=�$��Center=�$��5,0,0,0q�$��2���� $��3�����#888888� -   $ PlayerDisplayGrid��$��3�� �� $��5* ��� $��1* ��� $��5* ��� $��1* ��� $��5* �.+��� -   $ FirstPlayerLabel��$��Times New Roman)�$
��Bold"�$��30(�$��Center=�$��Center=�$��0���� $��>��$��Times New Roman)�$
��Bold"�$��30(�$��Center=�$��Center=�$��1���� -   $ SecondPlayerLabel��$��Times New Roman)�$��20(�$��Center=�$��Center=�$��2���� $��>��$��Times New Roman)�$
��Bold"�$��30(�$��Center=�$��Center=�$��3���� -   $ ThirdPlayerLabel��$��Times New Roman)�$��20(�$��Center=�$��Center=�$��4���� -   $ LogLabel��$��[Log]��$��Times New Roman)�$��30(�$
��Left=�$��Center=�$��5,0,0,0q�$��4��� -   $ NavigationGrid�����#aaaaaa$��1�� �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� -   $ 	NewButton��$	��New��$��Times New Roman)�$��30(�$��Stretch=�$��Stretch=�$��0���� -   $ 	FitButton��$��Fit on Screen��$��Times New Roman)�$��30(�$��Stretch=�$��Stretch=�$��6���� $��Width:��$��Times New Roman)�$��30(�$
��Left=�$��Center=�$��5,0,0,0q�$��1���� $��Height:��$��Times New Roman)�$��30(�$��Right=�$��Center=�$��0,0,65,0q�$��1���� $��Mode:��$��Times New Roman)�$��30(�$
��Left=�$��Center=�$��5,0,0,0q�$��2���� $��Moku:��$��Times New Roman)�$��30(�$
��Left=�$��Center=�$��5,0,0,0q�$��3���� $��	Activate:��$��Times New Roman)�$��30(�$��Right=�$��Center=�$��0,0,45,0q�$��3���� $��Move:��$��Times New Roman)�$��30(�$
��Left=�$��Center=�$��5,0,0,0q�$��4���� $��Overlay:��$��Times New Roman)�$��30(�$��Right=�$��Center=�$��0,0,45,0q�$��4���� $��Player Number:��$��Times New Roman)�$��30(�$
��Left=�$��Center=�$��5,0,0,0q�$��5���� -    ��Name$ 	WidthText��$��19��$��50��$��40��$��Times New Roman)�$��30(�$
��Left=�$��Center=�$��90,0,0,0q�$��1���� -   $ 
HeightText��$��19��$��50��$��40��$��Times New Roman)�$��30(�$��Right=�$��Center=�$��0,0,10,0q�$��1���� -   $ MokuText��$��5��$��50��$��40��$��Times New Roman)�$��30(�$
��Left=�$��Center=�$��90,0,0,0q�$��3���� -   $ MoveText��$��1��$��50��$��40��$��Times New Roman)�$��30(�$
��Left=�$��Center=�$��90,0,0,0q�$��4���� -   $ 
PlayerText��$��2��$��50��$��40��$��Times New Roman)�$��30(�$
��Left=�$��Center=�$��	200,0,0,0q�$��5���� -    	 ��Name$	 ModeComboBox��
 �� SelectedIndex$
 1��$	��150��$��40��$��Times New Roman)�$��30(�$��Center=�$
��Left=�$��90,0,0,0q�$��2��.@��� .��	Analyse�� .��Normal�� .��Moku�� .��	Scatter�� .��
Diagonal�� -!    ��Name$ ActivateMokuCheckBox��$��Right=�$��Center=�$��0,0,10,0q�$��3�� � LayoutTransform ��  �� ScaleX$ 2Q� �� ScaleY$ 2Q��� -"   $ OverlayCheckBox��$��Right=�$��Center=�$��0,0,10,0q�$��4�� �� $ 2Q�$ 2Q�