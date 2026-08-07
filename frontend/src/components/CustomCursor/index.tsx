import { useEffect, useRef, useState } from 'react';
import { useMicroSounds } from '../../hooks/useMicroSounds';
import styles from './CustomCursor.module.css';

export const CustomCursor = () => {
  const dotRef  = useRef<HTMLDivElement>(null);
  const ringRef = useRef<HTMLDivElement>(null);
  const wrapRef = useRef<HTMLDivElement>(null);
  const [hovered,  setHovered]  = useState(false);
  const [clicking, setClicking] = useState(false);
  const { playHover, playClick } = useMicroSounds();

  useEffect(() => {
    // Hide default cursor
    document.body.style.cursor = 'none';

    let animFrame: number;
    let rx = 0, ry = 0;       // ring position (lerped)
    let tx = 0, ty = 0;       // target (mouse)

    const onMove = (e: MouseEvent) => {
      tx = e.clientX;
      ty = e.clientY;
      // Dot follows immediately
      if (dotRef.current) {
        dotRef.current.style.left = `${tx}px`;
        dotRef.current.style.top  = `${ty}px`;
      }
    };

    const lerp = (a: number, b: number, t: number) => a + (b - a) * t;

    const loop = () => {
      rx = lerp(rx, tx, 0.12);
      ry = lerp(ry, ty, 0.12);
      if (ringRef.current) {
        ringRef.current.style.left = `${rx}px`;
        ringRef.current.style.top  = `${ry}px`;
      }
      animFrame = requestAnimationFrame(loop);
    };

    const onDown  = () => { setClicking(true); playClick(); };
    const onUp    = () => setClicking(false);

    const onOver  = (e: MouseEvent) => {
      const el = e.target as HTMLElement;
      const isInteractive = el.closest('a, button, [role="button"], input, select, textarea') !== null;
      setHovered(prev => {
        if (!prev && isInteractive) playHover();
        return isInteractive;
      });
    };

    document.addEventListener('mousemove', onMove);
    document.addEventListener('mouseover', onOver);
    document.addEventListener('mousedown', onDown);
    document.addEventListener('mouseup',   onUp);
    animFrame = requestAnimationFrame(loop);

    return () => {
      document.body.style.cursor = '';
      document.removeEventListener('mousemove', onMove);
      document.removeEventListener('mouseover', onOver);
      document.removeEventListener('mousedown', onDown);
      document.removeEventListener('mouseup',   onUp);
      cancelAnimationFrame(animFrame);
    };
  }, []);

  return (
    <div
      ref={wrapRef}
      className={`${styles.cursor} ${hovered ? styles.hovered : ''} ${clicking ? styles.clicking : ''}`}
      aria-hidden="true"
    >
      <div ref={dotRef}  className={styles.dot}  />
      <div ref={ringRef} className={styles.ring} />
    </div>
  );
};
