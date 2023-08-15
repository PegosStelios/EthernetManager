# EthernetManager

`EthernetManager` is a simple console application that allows users to enable, disable, or abort changes to the Ethernet adapter on a Windows machine.

## Features

- **Elevated Privileges**: The application automatically requests elevated (administrator) privileges if not already running with them.
- **User-Friendly**: Provides clear prompts to the user for enabling, disabling, or aborting changes to the Ethernet adapter.
- **Safe**: Only targets the Ethernet adapter, ensuring other network adapters remain unaffected.

## Usage

1. Run the `EthernetManager.exe` application.
2. You will be prompted with the following message:
Do you want to enable, disable the Ethernet adapter, or abort? (Enter 'e' for enable, 'd' for disable, or 'a' for abort)

3. Enter your choice:
- `e`: Enable the Ethernet adapter.
- `d`: Disable the Ethernet adapter.
- `a`: Abort the operation and exit the application.

## Requirements

- Windows OS
- .NET Framework (version used to compile the application)

## Building from Source

1. Clone the repository or download the source code.
2. Open the solution in Visual Studio.
3. Build the solution.

## Contributing

If you'd like to contribute to the development of `EthernetManager`, please fork the repository and submit a pull request.

## License

This project is licensed under the GNU General Public License v3.0. See the `LICENSE` file for details.