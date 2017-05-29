
using Gabriel.Cat;
using Gabriel.Cat.Extension;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PokemonGBAFrameWork;
namespace SistemaMTBW
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RomData rom; 
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
        	if(MessageBox.Show("Esta aplicacion hece que puedas impedir mediante script que se pueda capturar un pokemon créditos a FBI de PokemonCommunity por el código ASM\n¿Quieres ver el código fuente?","Sobre la App",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
        		System.Diagnostics.Process.Start("https://github.com/TetradogPokemonGBA/ImpedirQueUnPokemonSeaCaturable");
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opnRom = new OpenFileDialog();
            opnRom.Filter = "Rom Pokemon GBA|*.gba";
            try
            {
                if (opnRom.ShowDialog().GetValueOrDefault())
                {
                    rom = new RomData(opnRom.FileName);
                    PonTexto();
                   
                    switch(rom.Edicion.AbreviacionRom)
                    {
                        case AbreviacionCanon.BPR: imgDecoración.SetImage(Imagenes.PokeballRojoFuego); break;
                        case AbreviacionCanon.BPG: imgDecoración.SetImage(Imagenes.PokeballVerdeHoja); break;
                        default:throw new Exception();
                    }
                     btnImpedirCaptura.IsEnabled = true; 
                }
                else if(rom!=null)
                {
                    MessageBox.Show("No se ha cambiado la rom","Atención",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                }else
                {
                    MessageBox.Show("No se ha cargado nada...");
                }
            }catch
            {
                btnImpedirCaptura.IsEnabled = false;
                rom = null;
                imgDecoración.SetImage(new Bitmap(1, 1));
                MessageBox.Show("La rom no es compatible","Solo acepta ediciones de Kanto");
            }
        }

        private void PonTexto()
        {
        	if(PokemonGBAFrameWork.QuitarTutorialBatallaOak.EstaActivado(rom))
            {
                btnImpedirCaptura.Content = "Sistema captura original ";
            }
           else
            {
                btnImpedirCaptura.Content = "Impedir captura";
            }
        }

        private void btnPonerOQuitar_Click(object sender, RoutedEventArgs e)
        {
        	PokemonGBAFrameWork.ImpedirCapturaViaScript.Variable=(int)(Hex)txtVariable.Text;//me falta sacar la variable al cargar si esta activada...
            if (PokemonGBAFrameWork.ImpedirCapturaViaScript.EstaActivado(rom))
            {
            	PokemonGBAFrameWork.ImpedirCapturaViaScript.Desactivar(rom);
             
            }
            else
            {
              PokemonGBAFrameWork.ImpedirCapturaViaScript.Activar(rom);
   
            }
            PonTexto();
            try{
            	rom.Rom.Save();
            }
            catch{
            	if(MessageBox.Show("No se ha podido guardar los cambios...puede que otro programa tenga la rom ocupada\nDesas guardar un backup?","Hay problemas",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            		rom.Rom.BackUp();
            }
        }
    }
}
