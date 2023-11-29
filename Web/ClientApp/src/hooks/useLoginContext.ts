import { LoginContext } from "./LoginContext";
import {useContext} from "react";

export const useLoginContext = () => useContext(LoginContext);