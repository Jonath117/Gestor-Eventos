import { execSync } from "child_process";

const files = process.argv.slice(2);

// Filtrar solo archivos C#
const csFiles = files.filter(f => f.endsWith(".cs"));

if (csFiles.length === 0) {
  process.exit(0);
}

const filesArg = csFiles.join(" ");

try {
  execSync(`dotnet format backend/backend.slnx --include ${filesArg}`, {
    stdio: "inherit",
  });
} catch (e) {
  process.exit(1);
}