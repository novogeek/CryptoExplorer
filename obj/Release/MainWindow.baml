   M S B A M L   `   `   `  ���� I  ECryptoExplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:CryptoExplorerCryptoExplorer      CryptoExplorer.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:CryptoExplorer       � Title$  Crypto Explorer��$��577.159��$��697.6��.��� $��Center=�$	��482��$��	20,10,0,0q�$	��Top=�$	��651�� � ColumnDefinitions �� $
��650* ��� .+��� -    ��Name$ 
tabControl��$
��Left=�$	��529��$��-16,0,0,-47q�$	��Top=�$	��667��$��2��.@��� $��Bitwise Operations��.��� 
�������$��0,0,-0.4,-0.4q� �� $��8* ��� $	��53* ��� $
��155* ��� $
��446* �.+��� -    ��Name$ btnBrowseBitwisePic1��$��Browse Source��$
��Left=�$��
285,47,0,0q�$	��Top=�$��91��$��18��$��3���� -    ��Name$ txtBitwisePic1FileName�� �� 
IsReadOnly . $
��Left=�$��19��$��	27,46,0,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$	��446��$��3��$��1���� -   $ btnBrowseBitwisePic2��$��
Browse Key��$
��Left=�$��
285,79,0,0q�$	��Top=�$��91��$��18��$��3���� -   $ txtBitwisePic2FileName�� . $
��Left=�$��19��$��	27,78,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��446��$��3��$��1���� -   $ btnComputeBO��$��Manipulate Source��$
��Left=�$��312,315,0,0q�$	��Top=�$	��115��$��19��$��3��	� -    	�Name$ rbBitwiseAnd�� 	� 	GroupName$ rbBitwiseOperator��$	��AND��$
��Left=�$��335,235,0,0q�$	��Top=�	 J� RenderTransformOrigin$	 1.868,0.382&�$��15��$��63��$��3��	� -   $ rbBitwiseOR��$ rbBitwiseOperator��$��OR��$
��Left=�$��335,260,0,0q�$	��Top=�$��15��$��65��$��3��	� -	   $ rbBitwiseXOR��$ rbBitwiseOperator��$	��XOR��$
��Left=�$��335,284,0,0q�$	��Top=�$��15��$��65��$��3���� -
   
 ��Name$
 lblBitwiseOp��$��Select Bitwise Operation��$
��Left=�$��299,199,0,0q�$	��Top=�$��26��$	��142��$��3���� -    ��Name$ imgBitwiseDisplay��$	��349��$��0.2,117,157.4,0q�$	��Top=�$��2��$��2���� $
��Left=�$��5,13,0,0q�$
��Wrap=�$��Bitwise Operators 101��$	��Top=�$��14(�$��3���� -   $ btnDisplayBitwiseSource��$��Display Source��$
��Left=�$��318,160,0,0q�$	��Top=�$��91��$��18��$��3���� $��Stream Ciphers��.��� 
�������$��
-1,0,0.6,0q� �� $	��20* ��� $	��99* ��� $��7* ��� $
��536* �.+��� -   $ btnBrowseSourcePic��$��Browse Source��$
��Left=�$��362.6,65,0,0q�$	��Top=�$��88��$��18��$��3���� -   $ txtFileNameSourcePic�� . $
��Left=�$��19��$��1.649,65,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��446��$��3��$��1���� -   $ btnBrowseKeyStream��$��
Browse Key��$
��Left=�$��362.6,101,0,0q�$	��Top=�$��88��$��18��$��3���� -   $ txtFileNameKeyStream�� . $
��Left=�$��19��$��1.649,100,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��446��$��3��$��1���� -   $ btnEncryptDecryptStream��$��Encrypt/Decrypt��$
��Left=�$��404.6,187,0,0q�$	��Top=�$	��106��$��19��$��3���� -   $ btnSaveStream��$
��Save��$
��Left=�$��404.6,225,0,0q�$	��Top=�$	��106��$��19��$��3���� -   $ imgDisplayStream��$	��349��$��46.649,132,150.4,0q�$	��Top=�$��3��$��1���� $
��Left=�$��111.6,20,0,0q�$
��Wrap=�$ ��Stream Cipher Reuse Attack��$	��Top=�$��14(�$��3���� $��Block Ciphers��.��� 
�������$��0,0,-0.4,-0.4q� �� $��8* ��� $	��53* ��� $
��155* ��� $
��446* �.+��� -   $ 	btnBrowse��$��Browse��$
��Left=�$��284.713,54,0,0q�$	��Top=�$��75��$��18��$��3���� -   $ 
btnDecrypt��$��Decrypt��$
��Left=�$��80.2,93,0,0q�$	��Top=�$��75��$	 0.679,-2.418&�$��19��$��2���� -   $ 
btnEncrypt��$��Encrypt��$
��Left=�$��	41,93,0,0q�$	��Top=�$��75��$��19��$��2��$��1���� -   $ txtFileName�� . $
��Left=�$��19��$��	27,53,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��446��$��3��$��1��	� -   $ rbEncryptionModeECB��$ rbEncryptionMode�� d� 	IsChecked$
 TrueY�$��AES ECB��$
��Left=�$��173.713,96,0,0q�$	��Top=�$	 1.868,0.382&�$��15��$��63��$��3��	� -   $ rbEncryptionModeCBC��$ rbEncryptionMode��$��AES CBC��$
��Left=�$��264.713,95,0,0q�$	��Top=�$��15��$��65��$��3���� -   $
 label��$��Encryption Mode:��$
��Left=�$��51.713,90,0,0q�$	��Top=�$��26��$	��104��$��3���� -   $ 
imgDisplay��$	��349��$��63.2,137,94.4,0q�$	��Top=�$��2��$��2���� $
��Left=�$��140.2,17,0,0q�$
��Wrap=�$.��(Frequency Analysis on Block Cipher modes��$	��Top=�$��14(�$��2��$��2���� $��Padding Oracle��$
��Left=�$��20��$��Bottom=�$��93��$��-2.2,-2,-36.4,2q�.��� ���	#FFE5E5E5.+�� 
�������$��0,0,-0.4,-0.4q� �� $��8* ��� $	��53* ��� $
��602* �.+��� -   $ 	btnAttack��$��Attack!��$
��Left=�$��215.2,109,0,0q�$	��Top=�$��75��$��18��$��2��$��17���� -   $ 
txtBaseUrl��$1��+http://localhost.:1337/Home/compute?secret=��$
��Left=�$��19��$��25.2,54,0,0q�$
 Wrap=�$	��Top=�$	��557��$��2��$��15���� $
��Left=�$��161.2,18,0,0q�$
��Wrap=�$��Padding Oracle Attack��$	��Top=�$��14(�$��2���� $��1��$
��Left=�$��9,54,0,0q�$
��Wrap=�$��Base URL��$	��Top=�$��2��$��64���� $��1��$
��Left=�$��9,82,0,0q�$
��Wrap=�$��
Cipher Hex��$	��Top=�$��2��$��64���� -   $ txtCipherHex��$�� ��$
��Left=�$��19��$��25.2,79,0,0q�$
 Wrap=�$	��Top=�$	��557��$��2��$��16���� -   $ txtOracleLogSummary�� .  �� AcceptsReturn . " �� VerticalScrollBarVisibility$
 Auto=�$
��Left=�$	��256��$��
33,152,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��602��$��1��$��2���� $��1��$
��Left=�$��
33,131,0,0q�$
��Wrap=�$��
Attack log��$	��Top=�$��2��$	��125���� $��Decrypted text��$��1��$
��Left=�$��
29,409,0,0q�$	��Top=�$	 -1.03,0.457&�$��2���� -    $ txtDecryptedText��$��1��$
��Left=�$��51��$��
33,439,0,0q�$
 Wrap=�$	��Top=�$	��602��$��2��$��16(����	#FFE82222