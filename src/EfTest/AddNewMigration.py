import subprocess

# Get user input for the migration name
user_input = input("Enter the migration name: ")

# Define the command to execute
command = f"dotnet ef migrations add {user_input} -o EntityFramework/Migrations"

# Execute the command
try:
    subprocess.run(command, shell=True, check=True)
    print(f"Migration '{user_input}' added successfully.")
except subprocess.CalledProcessError as e:
    print(f"Error: {e}")
except KeyboardInterrupt:
    print("Operation aborted by user.")

input("Type something to close console...")