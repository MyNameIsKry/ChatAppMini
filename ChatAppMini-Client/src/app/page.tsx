
import Link from 'next/link';

export default function Home() {
  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-50">
      <div className="text-center">
        <h1 className="text-4xl font-bold mb-6">Welcome to ChatApp Mini</h1>
        <div className="space-x-4">
          <Link 
            href="/login"
            className="inline-block px-6 py-3 text-white bg-blue-600 rounded-md hover:bg-blue-700"
          >
            Login
          </Link>
          <Link
            href="/register"
            className="inline-block px-6 py-3 text-blue-600 bg-white border border-blue-600 rounded-md hover:bg-blue-50"
          >
            Register
          </Link>
        </div>
      </div>
    </div>
  );
}
