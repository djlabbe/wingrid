import {existsSync} from "fs";
import { spawn } from "child_process";

const certFilePath = "./localhost.pem";
const keyFilePath = "./localhost.key";

if (!existsSync(certFilePath) || !existsSync(keyFilePath)) {
    spawn("dotnet", ["dev-certs", "https", "--export-path", certFilePath, "--format", "Pem", "--no-password"], {
        stdio: "inherit"
    }).on("exit", (code) => process.exit(code));
}