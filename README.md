# Chapin Warriors – Sistema de Control (Proyecto 1, IPC2)

Sistema web en **C# / ASP.NET Core MVC** para cargar ciudades y robots desde un
archivo XML, y ejecutar misiones de **rescate** o **extraccion de recursos**
sobre la malla de celdas de cada ciudad, mostrando la ruta encontrada y el
codigo Graphviz correspondiente.

## Contenido del repositorio

```
ChapinWarriors.csproj                   Proyecto .NET (net8.0)
Program.cs                              Punto de entrada / configuracion MVC
Estructuras/                            TDA propios: Nodo<T>, ListaSimple<T>, Pila<T>
Modelos/                                Celda, Ciudad, Robot (+ RobotRescate/RobotFighter), ResultadoMision
Servicios/                              CargadorConfiguracion, AlmacenDatos, MotorMisiones, GeneradorGraphviz
Controllers/                            HomeController, ConfiguracionController, MisionesController
Views/                                  Vistas Razor (.cshtml)
wwwroot/css/site.css                    Estilos propios (sin frameworks CSS externos)
ejemplo_configuracion.xml               XML pequeno para pruebas rapidas
pruebas_completas.xml                   XML que cubre todos los casos de mision (exito/imposible, debil/fuerte, etc.)
Ensayo_ProyectoChapinWarriors.md        Ensayo del proyecto (formato del auxiliar), con espacios marcados para los diagramas
Prompt_Lucidchart_Diagramas.md          Prompts listos para generar el diagrama de clases y los diagramas de actividades en Lucidchart
```

## Como correrlo

1. Necesitas el **.NET SDK 8** instalado (`dotnet --version`).
2. Desde la carpeta del proyecto:
   ```
   dotnet run
   ```
3. Abre la URL que muestre la consola (algo como `http://localhost:5xxx`).
4. Ve a **Configuracion** y sube `pruebas_completas.xml` (o `ejemplo_configuracion.xml`)
   para probar rapido, o tu propio archivo con el formato del enunciado.
5. Ve a **Misiones**, elige tipo de mision, ciudad, robot y objetivo, y
   ejecuta. Veras la ruta pintada sobre la malla y el codigo `.dot`.

## Arquitectura (MVC)

- **Estructuras/**: TDA propios (`Nodo<T>`, `ListaSimple<T>`, `Pila<T>`). No se usa
  `List`, `Queue`, `Stack` ni `LinkedList` de C# en ninguna parte del dominio.
- **Modelos/**: representan el dominio del problema (celdas, ciudad como lista de
  listas, robots con herencia, resultado de mision).
- **Servicios/**: logica de negocio pura, sin dependencias de ASP.NET —
  parseo del XML, busqueda de rutas (BFS para rescate, Dijkstra/SPFA adaptado
  para extraccion) y generacion del codigo Graphviz.
- **Controllers/Views/**: capa web delgada que solo orquesta llamadas a los
  servicios y muestra los resultados.

