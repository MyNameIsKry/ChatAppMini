import { MessageCircle } from 'lucide-react';

interface LoadingProps {
  message?: string;
}

export default function Loading({ message = "Đang tải..." }: LoadingProps) {
  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center">
      <div className="text-center">
        <div className="relative">
          <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-blue-600"></div>
          <MessageCircle className="h-8 w-8 text-blue-600 absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2" />
        </div>
        <p className="mt-4 text-gray-600">{message}</p>
      </div>
    </div>
  );
}
