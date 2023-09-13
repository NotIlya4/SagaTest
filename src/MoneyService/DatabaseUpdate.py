import subprocess

# Define the command to execute
command = f"dotnet ef database update"

# Execute the command
try:
    subprocess.run(command, shell=True, check=True)
    print(f"Database updated successfully.")
except subprocess.CalledProcessError as e:
    print(f"Error: {e}")

input("Type something to close console...")