# Tic-Tac-Toe Distribuido (.NET)

## Descripción General
Este proyecto implementa el clásico juego de Ta-Te-Ti (Tic-Tac-Toe) en una arquitectura distribuida utilizando .NET. Permite partidas entre dos usuarios o entre un usuario y la PC, conectándose a través de sockets TCP a un servidor central.

## Estructura del Proyecto
- **Server/**: Lógica del servidor, gestiona las partidas, turnos y comunicación entre clientes.
- **UserClient/**: Cliente para usuarios humanos, permite jugar desde consola.
- **PCClient/**: Cliente automático (PC), realiza jugadas aleatorias.
- **Shared/**: Código y modelos compartidos entre cliente y servidor (mensajes, configuración, enums, etc).
- **scripts/**: Scripts para facilitar la ejecución de los clientes y el servidor con un solo clic.

## Flujo de Funcionamiento
1. **Inicio del Servidor**: Ejecuta el servidor. Este queda a la espera de conexiones de clientes.
2. **Conexión de Clientes**: Dos clientes (UserClient o PCClient) se conectan al servidor. El servidor los empareja para una partida.
3. **Turnos**:
   - El servidor envía el estado del tablero y de la partida a ambos clientes.
   - El cliente cuyo turno es, recibe la indicación y debe enviar su jugada (posición 1-9).
   - El otro cliente ve el mensaje "Esperando al otro jugador...".
   - El servidor valida la jugada, actualiza el tablero y repite el ciclo.
4. **Fin de Partida**:
   - Cuando hay un ganador o empate, el servidor informa el resultado y pregunta si desean jugar de nuevo.
   - Si ambos aceptan, se reinicia la partida; si no, se cierra la conexión (no funciona aún).

## Detalles Técnicos
- **Comunicación**: TCP, mensajes serializados en JSON.
- **Colores**: Las X y O se muestran en colores configurables en consola.
- **Sincronización**: El servidor controla los turnos y asegura que solo el jugador correspondiente pueda mover.
- **Reinicio**: Al terminar una partida, ambos clientes deben aceptar para reiniciar.

## Ejecución Rápida
1. Compila la solución: `dotnet build distributed.sln`
2. Ejecuta el servidor: `dotnet run --project Server/Server.csproj`
3. Ejecuta dos clientes (en terminales separadas):
   - `dotnet run --project UserClient/UserClient.csproj`
   - o `dotnet run --project PCClient/PCClient.csproj`

## Notas
- Si un cliente no responde en su turno, el otro quedará esperando.
- Puedes jugar PC vs Usuario, Usuario vs Usuario o PC vs PC.
- El código es modular y fácilmente extensible para nuevas reglas o interfaces.

---
Autor: Luis Bustos
