import "@/app/globals.css"
import { Toaster } from "sonner";

export default async function AuthLayout({
    children
  }: Readonly<{
    children: React.ReactNode;
  }>) {
  
    return (
      <div>
          {children}
          <Toaster position="top-right" />
      </div>
    );
  }